import { IUserResult, ServerMessageTypes } from '../api/Types';
import { GetCookie, SetCookie, TOKEN_KEY } from '../utils/Cookies';
import Database from './Remote';

// Types and constants
const DefaultAvatarUrl: string = 'https://cdn.discordapp.com/attachments/836004673299021824/1107722671850004570/avatar.png';

export enum UserTypes {
	Guest = 0,
	Student = 1,
	Teacher = 2,
	Administrator = 100
}

export const UserTypeNames : { [id: number]: string } = {
	[UserTypes.Guest]: 'Guest',
	[UserTypes.Student]: 'Student',
	[UserTypes.Teacher]: 'Teacher',
	[UserTypes.Administrator]: 'Administrator'
}

export interface IUser {
	id: number,
	type: UserTypes,
	username: string,
	email: string,
	avatarUrl: string|undefined
}

export class Guest implements IUser
{
	id: number;
	type: UserTypes;
	username: string;
	email: string;
	avatarUrl: string;

	constructor(id: number, username: string, email: string, avatarUrl: string = DefaultAvatarUrl)
	{
		this.id = id;
		this.type = UserTypes.Guest;
		this.username = username;
		this.email = email;
		this.avatarUrl = avatarUrl;
	}
}

export class Student implements IUser
{
	id: number;
	type: UserTypes;
	username: string;
	email: string;
	avatarUrl: string;

	constructor(id: number, username: string, email: string, avatarUrl: string = DefaultAvatarUrl)
	{
		this.id = id;
		this.type = UserTypes.Student;
		this.username = username;
		this.email = email;
		this.avatarUrl = avatarUrl;
	}
}

export class Teacher implements IUser
{
	id: number;
	type: UserTypes;
	username: string;
	email: string;
	avatarUrl: string;

	constructor(id: number, username: string, email: string, avatarUrl: string = DefaultAvatarUrl)
	{
		this.id = id;
		this.type = UserTypes.Teacher;
		this.username = username;
		this.email = email;
		this.avatarUrl = avatarUrl;
	}
}

export class Administrator implements IUser
{
	id: number;
	type: UserTypes;
	username: string;
	email: string;
	avatarUrl: string;

	constructor(id: number, username: string, email: string, avatarUrl: string = DefaultAvatarUrl)
	{
		this.id = id;
		this.type = UserTypes.Administrator;
		this.username = username;
		this.email = email;
		this.avatarUrl = avatarUrl;
	}
}

export const DefaultUser: Guest = new Guest(-1, 'Guest', 'N/A', undefined);

// Variables
export let CurrentUserData: IUser = DefaultUser;
export let CurrentUserToken: string = '';

// Functions
export const setCurrentUserData = function(userData: IUser)
{
	CurrentUserData = userData;
}

export const getCurrentUserAsync = async function()
{
	let user: string | IUserResult = await Database.getByKeyAsync('user', [ CurrentUserToken ]);

	if (typeof(user) === 'string' || user.message.messageType === ServerMessageTypes.Error)
		return DefaultUser;

	return user.data;
}

export const matchesMinUserType = function(level: number, levels: number|object|null)
{
	if (levels === null)
		return true;

	if (typeof levels === 'number' && levels >= level)
		return true;

	if (Array.isArray(levels) && levels.includes(level))
		return true;

	return false;
}

export const isLoggedInAsync = async function()
{
	const currentUser = await getCurrentUserAsync();

	if (currentUser.type === UserTypes.Guest)
		return false;

	return true;
}

export const reloadCurrentUserToken = function()
{
	CurrentUserToken = GetCookie(TOKEN_KEY) ?? '';

	//LocalStorage.readFromLocalStorage(TOKEN_KEY) || '';
}

export const saveCurrentUserToken = function(token: string)
{
	SetCookie(TOKEN_KEY, token, 7);
}

export const deleteCurrentUserToken = function()
{
	SetCookie(TOKEN_KEY, '', 0);
}
