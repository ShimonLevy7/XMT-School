import './styles/App.css';
import 'react-toastify/dist/ReactToastify.css';

import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import { BrowserRouter, Route, Routes } from "react-router-dom";

import { Pages, IPage } from './ts/Pages';
import { matchesMinUserType, getCurrentUserAsync, UserTypes, reloadCurrentUserToken, IUser, DefaultUser, setCurrentUserData } from './ts/Users';
import Error401Page from './pages/Error401';
import Error403Page from './pages/Error403';
import Error404Page from './pages/Error404';
import NavBar from './components/NavBar';
import Loading from './components/Loading';

export default function App()
{
	// Keep page blank until all data is ready.
	const [ dataLoaded, setDataLoaded ]: [ boolean, Function ] = useState(false);

	// The data.
	const [ currentUser, setCurrentUserUser ]: [ IUser, Function ] = useState(DefaultUser);
	const [ routes, setRoutes ]: [ JSX.Element[] | undefined, Function ] = useState(undefined);

	useEffect(() => {
		const fetchDataAsync = async () => {
			setDataLoaded(false);

			reloadCurrentUserToken();

			// Fetch currently logged in user.
			const newCurrentUser = await getCurrentUserAsync();

			setCurrentUserUser(newCurrentUser); // Saves for this file.
			setCurrentUserData(newCurrentUser); // Saves in "Users" file, used by other files.

			// Fetch routes.
			const newRoutes = Pages.map((page, i) => {
				if (!page.href || !page.component)
					return (<Route key={i}></Route>);

				return (<Route key={i} path={page.href} element={getPageContent(newCurrentUser, page)} />);
			});

			setRoutes(newRoutes);

			// Complete.
			setDataLoaded(true);
		};

		fetchDataAsync();
	}, [ ]);

	return (<div>
		{dataLoaded
			? <>
				<NavBar user={currentUser}/>

				<br/>

				<BrowserRouter>
					<Routes>

					{routes}

					<Route path="*" element={<Error404Page />} />

					</Routes>
				</BrowserRouter>
			</>
			: <Loading></Loading>}
	</div>);
}


export const MAX_TEST_POINTS: number = 100;

export const DEFAULT_TOAST_OPTIONS = {
	position: toast.POSITION.BOTTOM_RIGHT
}

const getPageContent = function(currentUser: IUser, page: IPage)
{
	if (!page)
		return Error404Page();

	if (page.component)
	{
		if (!matchesMinUserType(currentUser.type, page.userTypes))
		{
			if (currentUser.type === UserTypes.Guest)
				return Error401Page();

			return Error403Page();
		}

		return React.cloneElement(page.component, { ...page.props });
	}
}
