/** @type {import('next').NextConfig} */
const path = require('path')

const nextConfig = {
  reactStrictMode: true,
  images: {
    domains: ["m.media-amazon.com"],
  },
  sassOptions: {
    includePaths: [path.join(__dirname, 'styles')],
  }
};


module.exports = nextConfig
