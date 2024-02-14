import '../styles/NavBar.css';

import { IPage, getPagesForUserType } from '../ts/Pages';
import type { IUser } from '../ts/Users';
import UserInfo from './UserInfo';

export default function NavBar({ user }: { user: IUser }) {
	let PagesArray = getPagesForUserType(user.type);

	return (<>
		<nav className="navbar navbar-expand-lg navbar-dark" style={{ backgroundColor: "#181818" }}>
			<a className="navbar-brand" href="/" title={document.title}>{document.title}</a>

			<button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
				<span className="navbar-toggler-icon"></span>
			</button>

			<div className="collapse navbar-collapse" id="navbarSupportedContent">
				<ul className="navbar-nav mr-auto">
					{PagesArray.map((page: IPage, i: number) =>
						<li key={i} className='nav-item'>
							<a className={`nav-link ${document.location.pathname === page.href ? 'active' : ''}`} href={page.href ?? undefined} title={page.title} onClick={page.func ?? undefined}>
								{page.icon ? page.icon({ size: 16 }) : ''} {page.title}
							</a>
						</li>
					)}
				</ul>

				<form className="form-inline my-2 my-lg-0">
					<span className='mt-2'>
						<UserInfo user={user}/>
					</span>
				</form>
			</div>
		</nav>
	</>);
}
