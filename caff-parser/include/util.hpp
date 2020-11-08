#ifndef UTIL_HPP
#define UTIL_HPP

#include "parsing_error.hpp"

#include <cstdint>
#include <istream>
#include <string>

namespace cp {
template<std::size_t N>
std::string read(std::istream &is)
{
    std::string data;
    data.reserve(N);
    for (std::size_t i = 0U; i < N; ++i) {
        char c;
        if (!(is.read(&c, 1))) {
            throw ParsingError("reading EOF");
        }
        data.push_back(c);
    }

    return data;
}

inline std::string read(std::istream &is, std::size_t n)
{
    std::string data;
    data.reserve(n);
    for (std::size_t i = 0U; i < n; ++i) {
        char c;
        if (!(is.read(&c, 1))) {
            throw ParsingError("reading EOF");
        }
        data.push_back(c);
    }

    return data;
}

inline std::string readUntilChar(std::istream &is, char endChar, std::size_t max)
{
    std::string data;
    for (std::size_t i = 0U; i < max; ++i) {
        char c;
        if (!(is.read(&c, 1))) {
            throw ParsingError("reading EOF");
        }
        if (c == endChar) {
            break;
        }
        data.push_back(c);
    }
    if (data.size() == max) {
        throw ParsingError("reached max but not found end char");
    }

    return data;
}

inline std::string readLine(std::istream &is, std::size_t max)
{
    return readUntilChar(is, '\n', max);
}

template<class T>
T bytesToInt(const std::string &str)
{
    static_assert(std::is_integral_v<T>);

    if (str.size() > sizeof(T)) {
        throw ParsingError("too many bytes");
    }

    uint64_t number = 0;

    for (uint8_t i = 0; i < str.size(); ++i) {
        number <<= 8;
        number += static_cast<uint8_t>(str[str.size() - 1 - i]);
    }

    return static_cast<T>(number);
}
} // namespace cp

#endif
