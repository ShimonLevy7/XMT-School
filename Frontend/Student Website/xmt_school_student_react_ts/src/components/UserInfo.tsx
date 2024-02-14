import '../styles/UserInfo.css';

import { useState, useEffect } from 'react';
import { FaUser } from "react-icons/fa";
import { IUser, UserTypeNames, isLoggedInAsync } from '../ts/Users';

export default function UserInfo({ user }: { user: IUser })
{
	const [ isLoggedIn, setIsLoggedIn ]: [ boolean, Function ] = useState(false);

	useEffect(() => {
		const fetchUser = async () => {
			const currentUser = await isLoggedInAsync();

			setIsLoggedIn(currentUser);
		};

		fetchUser();
	}, []);

	return (<>
		{isLoggedIn
			? <p className='userInfo noSelect'><FaUser color='rgb(96,  255, 96)'/> Logged in as <span className='bold select'>{user.username}</span> (<span className='italic select'>{UserTypeNames[user.type]} Account</span>)</p>
			: <p className='userInfo noSelect'><FaUser color='rgb(255, 96,  96)'/> Not logged in</p>
		}
	</>);
}
