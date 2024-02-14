import { FaHome, FaInfo, FaSignInAlt, FaSignOutAlt, FaUserCog, FaSchool } from "react-icons/fa";

import HomePage from "../pages/Home";
import AboutPage from "../pages/About";
import TakeATestPage from "../pages/TakeATestPage";
import LoginPage from "../pages/LoginPage";
import ProfilePage from "../pages/ProfilePage";

import { matchesMinUserType, UserTypes } from "./Users";
import { LogoutAsync } from "../utils/FuncUtils";
import TestsManagerPage from "../pages/TestsManagerPage";

interface IPage {
	title: string;
	icon: Function;
	href: string|null;
	component: JSX.Element|null;
	isDefault: boolean;
	func: React.MouseEventHandler|null;
	props: object|null;
	userTypes: Array<number>|null;
}

const Pages: Array<IPage> = [
	{
		title: 'Home',
		href: '/',
		isDefault: true,
		icon: FaHome,
		component: <HomePage/>,
		func: null,
		props: null,
		userTypes: null
	},
	{
		title: 'About',
		href: '/about',
		isDefault: false,
		icon: FaInfo,
		component: <AboutPage/>,
		func: null,
		props: null,
		userTypes: null
	},
	{
		title: 'Take a Test',
		href: '/take_a_test',
		isDefault: false,
		icon: FaSchool,
		component: <TakeATestPage/>,
		func: null,
		props: null,
		userTypes: [ UserTypes.Student, UserTypes.Administrator ]
	},
	{
		title: 'Tests Manager',
		href: '/tests_manager',
		isDefault: false,
		icon: FaSchool,
		component: <TestsManagerPage/>,
		func: null,
		props: null,
		userTypes: [ UserTypes.Teacher, UserTypes.Administrator ]
	},
	{
		title: 'Login',
		href: '/login',
		isDefault: false,
		icon: FaSignInAlt,
		component: <LoginPage/>,
		func: null,
		props: null,
		userTypes: [ UserTypes.Guest ]
	},
	{
		title: 'My Profile',
		href: '/profile',
		isDefault: false,
		icon: FaUserCog,
		component: <ProfilePage/>,
		func: null,
		props: null,
		userTypes: [ UserTypes.Student, UserTypes.Teacher, UserTypes.Administrator ]
	},
	{
		title: 'Logout',
		href: null,
		isDefault: false,
		icon: FaSignOutAlt,
		component: null,
		func: LogoutAsync,
		props: null,
		userTypes: [ UserTypes.Student, UserTypes.Teacher, UserTypes.Administrator ]
	}
];

const getPagesForUserType = function(userType: number)
{
	const pages = [];

	for (let i = 0; i < Pages.length; i++) {
		const page = Pages[i];

		if (matchesMinUserType(userType, page.userTypes))
			pages.push(page);
	}

	return pages;
}

export { getPagesForUserType };
export { Pages };
export type { IPage };
