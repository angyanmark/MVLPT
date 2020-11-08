#ifndef CAFF_HPP
#define CAFF_HPP

#include "ciff.hpp"

#include <cstdint>
#include <iostream>
#include <string>
#include <vector>

namespace cp {
class Caff {
    uint16_t year;
    uint8_t month;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;

    std::string creator;

    std::vector<std::pair<Ciff, uint64_t>> frames;

public:
    Caff();
    Caff(std::istream &is);

    void parse(std::istream &is);

    friend std::ostream &operator<<(std::ostream &, const Caff &);
};

std::ostream &operator<<(std::ostream &os, const Caff &ciff);
} // namespace cp

#endif
