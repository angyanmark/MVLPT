#include "caff.hpp"

#include <array>
#include <fstream>

#include "parsing_error.hpp"

int main(int argc, const char *argv[])
{
    if (argc != 2) {
        std::cerr << "Missing FILE argument.\n";
        return -1;
    }

    std::array<char, 1024 * 1024> buff;

    std::string filePath = argv[1];
    std::ifstream in(filePath, std::ifstream::binary);
    in.rdbuf()->pubsetbuf(buff.data(), buff.size());

    try {
        cp::Caff caff(in);

        std::cout << caff << '\n';
    } catch (const cp::ParsingError &e) {
        std::cerr << "Could not parse file: " << e.what() << '\n';
        return -1;
    } catch (...) {
        std::cerr << "Something went wrong\n";
        return -2;
    }

    return 0;
}
