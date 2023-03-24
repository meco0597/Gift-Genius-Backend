/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: false,
  webpackDevMiddleware: config => {
    config.watchOptions = {
      poll: 300,
      aggregateTimeout: 300,
    };
    return config;
  },
  images: {
    domains: ["m.media-amazon.com"],
  },
};


module.exports = nextConfig
