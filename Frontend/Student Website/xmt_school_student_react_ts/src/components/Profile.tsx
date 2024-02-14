import '../styles/Profile.css';

import { IUser, UserTypeNames } from '../ts/Users';

export default function Profile({ currentUser }: { currentUser: IUser })
{
	let avatarUrl = currentUser.avatarUrl;

	if (!currentUser.avatarUrl) // Has no avatar.
		avatarUrl = `https://ui-avatars.com/api/?name=${currentUser.username.replaceAll(' ', '+')}?size=128`

	let email = currentUser.email

	if (email === 'N/A')
		email = ''

	return (<>
		<div className="d-flex align-items-center justify-content-center">
			<div className='border border-primary rounded m-1 text-center container-fluid' style={{width:'30em'}}>
				<h3 className='title m-3'>My Profile</h3>

				<img className="avatar_image m-3" draggable={false} width={'128px'} src={avatarUrl} alt="User Avatar"/>

				<table className='d-flex align-items-center justify-content-center'>
					<tbody style={{textAlign: "left"}}>
						<tr>
							<th><label htmlFor="username">Username:</label></th>
							<td><p className='label m-2'>{currentUser.username}</p></td>
						</tr>

						<tr>
							<th><label htmlFor="email">E-Mail:</label></th>
							{email
								? <td><a className='label m-2' title={`Send an E-Mail to <${currentUser.email}>`} href={'mailto:' + currentUser.email}>{currentUser.email}</a></td>
								: <td><p className='label m-2'>N/A</p></td>
							}
						</tr>

						<tr>
							<th><label htmlFor="accountType">Account Type:</label></th>
							<td><p className='label m-2'>{UserTypeNames[currentUser.type]}</p></td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
	</>);
}
