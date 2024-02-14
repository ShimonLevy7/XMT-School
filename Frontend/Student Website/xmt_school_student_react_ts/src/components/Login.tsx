import '../styles/Site.css';
import '../styles/Login.css';

import { useState } from 'react';
import { toast, ToastContainer } from 'react-toastify';

import { DEFAULT_TOAST_OPTIONS } from '../App';
import { loginAsync } from '../api/Post';
import { ILoginResult } from '../api/Types';
import { reloadCurrentUserToken, saveCurrentUserToken } from '../ts/Users';
import { ServerMessageTypes } from '../api/Types';

export default function Login() {
	const [disableUi, setDisableUi] = useState(false);
	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");

	return (<>
		<div className="d-flex align-items-center justify-content-center">
			<div className='border border-primary rounded m-1 container-fluid' style={{ width: '30em' }}>
				<h3 className='text-center m-4'>Login</h3>

				<div style={{ display: 'flex', justifyContent: 'center' }}>

					<form className='w-50'>
						{/* <!-- Email input --> */}
						<div className="form-outline mb-4">
							<input disabled={disableUi} className="form-control" type='text' name="username" value={username} onChange={(e) => setUsername(e.target.value)} required={true}></input>
							<label className="form-label" htmlFor="username">Username</label>
						</div>

						{/* <!-- Password input --> */}
						<div className="form-outline mb-4">
							<input disabled={disableUi} className="form-control" type='password' name="password" value={password} onChange={(e) => setPassword(e.target.value)} required={true}></input>
							<label className="form-label" htmlFor="password">Password</label>
						</div>

						{/* <!-- Submit button --> */}

						<input disabled={disableUi} value={"Log in"} type="submit" className="btn btn-primary btn-block mb-4" onClick={async (e) => {
							e.preventDefault();

							setDisableUi(true);

							const result: string | ILoginResult = await loginAsync(username, password);

							if (typeof (result) === 'string')
							{
								setDisableUi(false);

								return toast.error(result, DEFAULT_TOAST_OPTIONS);
							}

							if (result.message.messageType === ServerMessageTypes.Error)
							{
								setDisableUi(false);

								return toast.error(`Error (${result.message.messageId}): ${result.message.message}`, DEFAULT_TOAST_OPTIONS);
							}

							setTimeout(() => {
								saveCurrentUserToken(result.data.tokenString);

								reloadCurrentUserToken();

								window.location.href = "/";
							}, 2500);

							return toast.success(`Login success!\nPlease wait...`, DEFAULT_TOAST_OPTIONS);
						}}/>
					</form>
				</div>

				<ToastContainer />
			</div>
		</div>
	</>);
}
