export default function Error401Page()
{

	return (<>
		<div className="centre">
			<h1 className="title_error">Error 401 - Unauthorized</h1>
			<p>You're not authorised to view the requested page.</p>

			<br/>

			<p>You can try to <a href="/login" className="link" title="Login">log in</a>, or return to the <a href="/" className="link" title="Home">home page</a>.</p>
		</div>
	</>);
}
