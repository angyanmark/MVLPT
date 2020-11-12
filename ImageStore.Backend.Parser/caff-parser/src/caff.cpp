#include "caff.hpp"

#include "parsing_error.hpp"
#include "util.hpp"

#include <set>

namespace cp {
Caff::Caff() : year(0), month(0), day(0), hour(0), minute(0)
{
}

Caff::Caff(std::istream &is)
{
    this->parse(is);
}

namespace {
uint64_t parseHeader(std::istream &is, uint64_t length)
{
    if (length != 20) {
        throw ParsingError("header length must be 20");
    }

    {
        auto magic = read<4>(is);
        if (magic != "CAFF") {
            throw ParsingError("magic bytes not found");
        }
    }

    {
        uint64_t headerSize = bytesToInt<uint64_t>(read<8>(is));
        if (headerSize != 20) {
            throw ParsingError("header size must be 20");
        }
    }

    uint64_t numberOfCIFFs = bytesToInt<uint64_t>(read<8>(is));
    return numberOfCIFFs;
}

struct CreditsData {
    uint16_t year;
    uint8_t month;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;

    std::string creator;
};

CreditsData parseCredits(std::istream &is, uint64_t length)
{
    if (length < 14) {
        throw ParsingError("credits length must be at least 14");
    }

    CreditsData cd;
    cd.year = bytesToInt<uint16_t>(read<2>(is));
    cd.month = bytesToInt<uint8_t>(read<1>(is));
    cd.day = bytesToInt<uint8_t>(read<1>(is));
    cd.hour = bytesToInt<uint8_t>(read<1>(is));
    cd.minute = bytesToInt<uint8_t>(read<1>(is));

    uint64_t creatorLength = bytesToInt<uint64_t>(read<8>(is));
    if (length - 14 != creatorLength) {
        throw ParsingError("creator length invalid");
    }

    cd.creator = read(is, creatorLength);
    return cd;
}

std::pair<Ciff, uint64_t> parseAnimation(std::istream &is, uint64_t length)
{
    if (length < 8) {
        throw ParsingError("not enough data");
    }

    uint64_t duration = bytesToInt<uint64_t>(read<8>(is));
    uint64_t bytesLeft = length;
    bytesLeft -= 8;
    Ciff ciff;
    auto bytesRead = ciff.parse(is, length);
    bytesLeft -= bytesRead;

    if (bytesLeft != 0) {
        throw ParsingError("invalid length");
    }

    return std::pair<Ciff, uint64_t>(std::move(ciff), duration);
}
} // namespace

void Caff::parse(std::istream &is)
{
    constexpr uint8_t header_id = 0x1;
    constexpr uint8_t credits_id = 0x2;
    constexpr uint8_t animation_id = 0x3;

    uint8_t id = bytesToInt<uint8_t>(read<1>(is));
    if (id != header_id) {
        throw ParsingError("file starting with bad block");
    }
    uint64_t length = bytesToInt<uint64_t>(read<8>(is));
    auto numberOfCIFFs = parseHeader(is, length);

    CreditsData cd;
    std::vector<std::pair<Ciff, uint64_t>> frames;

    bool creditsFound = false;
    for (uint8_t i = 0; i < numberOfCIFFs + 1; ++i) {
        uint8_t id = bytesToInt<uint8_t>(read<1>(is));
        uint64_t length = bytesToInt<uint64_t>(read<8>(is));

        if (id == credits_id) {
            if (creditsFound) {
                throw ParsingError("multiple credits block");
            }
            creditsFound = true;
            cd = parseCredits(is, length);
        } else if (id == animation_id) {
            frames.push_back(parseAnimation(is, length));
        }
    }

    this->creator = std::move(cd.creator);
    this->year = cd.year;
    this->month = cd.month;
    this->day = cd.day;
    this->hour = cd.hour;
    this->minute = cd.minute;
    this->frames = std::move(frames);
}

std::ostream &operator<<(std::ostream &os, const Caff &caff)
{
    std::set<std::string> captions;
    std::set<std::string> tags;

    for (const auto &frame : caff.frames) {
        captions.insert(frame.first.getCaption());
        const auto &tagsVector = frame.first.getTags();
        tags.insert(tagsVector.begin(), tagsVector.end());
    }

    os << captions.size() << '\n';
    for (const auto &caption : captions) {
        os << caption << '\n';
    }

    os << tags.size() << '\n';
    for (const auto &tag : tags) {
        os << tag << '\n';
    }

    if (caff.frames.size() > 0) {
        os << caff.frames[0].first;
    }

    return os;
}
} // namespace cp
