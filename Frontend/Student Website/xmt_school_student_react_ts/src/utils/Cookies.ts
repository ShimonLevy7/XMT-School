import { InDevMode } from "../api/Utils";

export const TOKEN_KEY = 'USER_TOKEN';

export function SetCookie(key: string, value: string, days: number)
{
    const expirationDate = new Date();
    expirationDate.setDate(expirationDate.getDate() + days);

    const cookieValue = encodeURIComponent(value) + (days ? `; expires=${expirationDate.toUTCString()}` : '');

    // Set domain attribute to allow cookies across subdomains
    const domain = InDevMode() ? 'localhost' : '.tiro-finale.com'; // Replace with your domain

    document.cookie = `${key}=${cookieValue}; path=/; domain=${domain};`;
}

export function GetCookie(name: string)
{
    const cookies = document.cookie.split('; ');

    for (const cookie of cookies)
	{
        const [cookieName, cookieValue] = cookie.split('=');

        if (cookieName === name)
		{

            return decodeURIComponent(cookieValue);
        }
    }

    return null;
}
