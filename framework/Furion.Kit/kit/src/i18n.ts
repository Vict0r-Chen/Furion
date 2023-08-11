import { i18n, Messages } from "@lingui/core";

export const locales: Record<string, string> = {
  "zh-hans": "简体中文",
  en: "English",
};

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

export async function dynamicActivate(locale: string) {
  const messages = await catalogs[locale as any]();
  i18n.loadAndActivate({ locale, messages });
}

export async function i18nInit(defaultLocale: string) {
  for (const local in locales) {
    await dynamicActivate(local);
  }

  i18n.activate(defaultLocale);
}
