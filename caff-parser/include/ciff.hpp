#ifndef CIFF_HPP
#define CIFF_HPP

#include <cstdint>
#include <iostream>
#include <string>
#include <vector>

namespace cp {
class Ciff {
    struct PixelData {
        uint8_t r;
        uint8_t g;
        uint8_t b;
        uint8_t a;
    };

    uint64_t width;
    uint64_t height;
    std::vector<PixelData> data;

    std::string caption;
    std::vector<std::string> tags;

public:
    Ciff();
    Ciff(std::istream &is, uint64_t maxLength);

    uint64_t parse(std::istream &is, uint64_t maxLength);

    const std::string &getCaption() const;
    const std::vector<std::string> &getTags() const;

    friend std::ostream &operator<<(std::ostream &, const Ciff &);
};

std::ostream &operator<<(std::ostream &os, const Ciff &ciff);
} // namespace cp

#endif
