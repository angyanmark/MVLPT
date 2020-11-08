#include "ciff.hpp"

#include "parsing_error.hpp"
#include "util.hpp"

#include <cassert>

#include "lodepng.h"

namespace cp {
Ciff::Ciff() : width(0U), height(0U)
{
}

Ciff::Ciff(std::istream &is, uint64_t maxLength)
{
    this->parse(is, maxLength);
}

uint64_t Ciff::parse(std::istream &is, uint64_t maxLength)
{
    if (maxLength < 37U) {
        throw ParsingError("not enough data");
    }

    // magic
    {
        auto magic = read<4>(is);
        if (magic != "CIFF") {
            throw ParsingError("magic bytes not found");
        }
    }

    // header size
    uint64_t headerSize = bytesToInt<uint64_t>(read<8>(is));
    if (headerSize > maxLength) {
        throw ParsingError("header size is too big");
    }
    // headerSize must be at least 37, 36 + 1 (\n)
    if (headerSize < 37U) {
        throw ParsingError("header size is invalid");
    }

    // content size
    uint64_t contentSize = bytesToInt<uint64_t>(read<8>(is));

    if (contentSize + headerSize > maxLength) {
        throw ParsingError("content size is too big");
    }

    if (contentSize % 3U != 0U) {
        throw ParsingError("content size is not devisiable by 3");
    }

    // width
    uint64_t width = bytesToInt<uint64_t>(read<8>(is));

    // height
    uint64_t height = bytesToInt<uint64_t>(read<8>(is));

    if (width != 0 && height != 0) {
        if (height > std::numeric_limits<uint64_t>::max() / 3 ||
          width > std::numeric_limits<uint64_t>::max() / (height * 3)) {
            throw ParsingError("invalid dimensions");
        }
    }

    if (contentSize != width * height * 3) {
        throw ParsingError("content size is invalid");
    }

    uint64_t bytesLeft = headerSize - 36;

    // caption
    std::string caption = readLine(is, bytesLeft);
    bytesLeft -= caption.size() + 1;

    // tags
    std::vector<std::string> tags;
    while (bytesLeft != 0U) {
        std::string tag = readUntilChar(is, '\0', bytesLeft);
        auto it = tag.find('\n');
        if (it != std::string::npos) {
            throw ParsingError("invalid tag");
        }
        bytesLeft -= tag.size() + 1;
        tags.push_back(std::move(tag));
    }

    // pixels
    std::vector<PixelData> pixels;
    uint64_t pixelCount = width * height;
    pixels.reserve(pixelCount);

    for (uint64_t i = 0; i < pixelCount; ++i) {
        PixelData pixel{};
        pixel.r = bytesToInt<uint8_t>(read<1>(is));
        pixel.g = bytesToInt<uint8_t>(read<1>(is));
        pixel.b = bytesToInt<uint8_t>(read<1>(is));
        pixel.a = 255;
        pixels.push_back(pixel);
    }

    this->width = width;
    this->height = height;
    this->data = std::move(pixels);
    this->caption = std::move(caption);
    this->tags = std::move(tags);

    return headerSize + contentSize;
}

const std::string &Ciff::getCaption() const
{
    return this->caption;
}
const std::vector<std::string> &Ciff::getTags() const
{
    return this->tags;
}

std::ostream &operator<<(std::ostream &os, const Ciff &ciff)
{
    if (ciff.width > std::numeric_limits<unsigned int>::max() ||
      ciff.height > std::numeric_limits<unsigned int>::max()) {
        throw ParsingError("cant convert raw image to png");
    }

    auto printByte = [&os](uint8_t byte) {
        static const char *hexNumbers = "0123456789ABCDEF";
        os << hexNumbers[(byte >> 4) & 0x0F] << hexNumbers[byte & 0x0F];
    };

    std::vector<unsigned char> pngImage;
    lodepng::encode(pngImage, reinterpret_cast<const unsigned char *>(ciff.data.data()),
      static_cast<unsigned int>(ciff.width), static_cast<unsigned int>(ciff.height));

    for (const auto &byte : pngImage) {
        printByte(byte);
    }

    return os;
}
} // namespace cp
