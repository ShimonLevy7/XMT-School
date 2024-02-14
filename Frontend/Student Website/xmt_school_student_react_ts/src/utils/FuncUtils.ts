import { logoutAsync } from "../api/Post";
import { CurrentUserData, deleteCurrentUserToken } from "../ts/Users";

export async function LogoutAsync()
{
	await logoutAsync(CurrentUserData.id);

	deleteCurrentUserToken();

	setTimeout(() => {
		window.location.href = "/";
	}, 100);
}

export function GetAgreedString(value: number, noun: string, noValue: boolean = false, ies: boolean = false): string {
    let unit: string;

    if (value === 1) {
        unit = noun;
    } else {
        const len = noun.length;

        if (ies && noun.charAt(len - 1) === 'y') {
            unit = noun.substring(0, len - 1) + 'ies';
        } else {
            unit = noun + 's';
        }
    }

    if (noValue) {
        return unit;
    } else {
        return value + ' ' + unit;
    }
}
