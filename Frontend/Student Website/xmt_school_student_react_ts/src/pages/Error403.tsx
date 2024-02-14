export default function Error403Page()
{

	return (<>
		<div className="centre">
			<h1 className="title_error">Error 403 - Forbidden</h1>
			<p>You're not authorised to view the requested page.</p>

			<br/>

			<p>Viewing this page requires elevated permissions that your user level does not have.</p>
			<p>Return to the <a href="/" className="link" title="Home">home page</a>?</p>
		</div>
	</>);
}
