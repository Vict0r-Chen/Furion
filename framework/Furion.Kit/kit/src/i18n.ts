import { i18n, Messages } from "@lingui/core";

export const locales = {
  en: "English",
  "zh-hans": "简体中文",
};
export const defaultLocale = "zh-hans";

const catalogs: Record<string, () => Promise<Messages>> = {
  en: async () => {
    const { messages } = await import(
      // @ts-ignore
      `./file.js!=!@lingui/loader!./locales/en/messages.po`
    );
    return messages;
  },
  "zh-hans": async () => {
    const { messages } = await import(
      // @ts-ignore
      `./file.js!=!@lingui/loader!./locales/zh-hans/messages.po`
    );
    return messages;
  },
};

/**
 * We do a dynamic import of just the catalog that we need
 * @param locale any locale string
 */
export async function dynamicActivate(locale: string) {
  const messages = await catalogs[locale as any]();
  i18n.loadAndActivate({ locale, messages });
}
