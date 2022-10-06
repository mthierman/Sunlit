import {
    provideFluentDesignSystem,
    fluentButton,
    baseLayerLuminance,
    StandardLuminance,
    accentPalette,
    neutralPalette,
    PaletteRGB,
    SwatchRGB,
    bodyFont,
} from "@fluentui/web-components";
import { parseColorHexRGB } from "@microsoft/fast-colors";
import * as icon from "./icon.js";

provideFluentDesignSystem().register(fluentButton());

bodyFont.withDefault("Segoe UI Variable Text");

const html = document.documentElement;

const setTheme = () => {
    let theme = matchMedia("(prefers-color-scheme: dark)").matches
        ? "dark"
        : "light";
    html.setAttribute("data-theme", theme);
    // localStorage.setItem("data-theme", theme);
};

const checkTheme = () => {
    window
        .matchMedia("(prefers-color-scheme: dark)")
        .addEventListener("change", () => setTheme());
};

setTheme();
checkTheme();
