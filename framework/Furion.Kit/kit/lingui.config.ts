/** @type {import('@lingui/conf').LinguiConfig} */
module.exports = {
  locales: ["zh-hans", "en"],
  sourceLocale: "zh-hans",
  catalogs: [
    {
      path: "<rootDir>/src/locales/{locale}/messages",
      include: ["src"],
    },
  ],
  format: "po",
};
