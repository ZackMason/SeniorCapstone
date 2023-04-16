#include <iostream>
#include <format>
#include <cmath>

float dot(float* v0, float* v1) {
    return v0[0]*v1[0]+v0[1]*v1[1];
}
float length2(float* v0) {
    return dot(v0, v0);
}
float length(float* v0) {
    return sqrtf(length2(v0));
}

int main(int argc, char** argv) {
    constexpr int image_size = 128;
    float image[image_size][image_size];

    for (size_t x = 0; x < image_size; x++) {
        for (size_t y = 0; y < image_size; y++) {
            image[x][y] = 0.0f;
        }
    }

    std::cout << std::format("P3\n{} {}\n255\n", image_size, image_size);

    for (size_t x = 0; x < image_size; x++) {
        for (size_t y = 0; y < image_size; y++) {
            float u[2];
            u[0] = ((float)x/float(image_size-1)) - 0.5f;
            u[1] = ((float)y/float(image_size-1)) - 0.5f;
            float d = fabsf(length(u) - 0.45f);

            int a = int((image[x][y] = 1.0f - sqrtf(d) * 4.0f) * 255.999f);
            std::cout << std::format("{} {} {}\n", a, a, a);
        }
    }

    return 0;
}