export default function Error400Page()
{

	return (<>
		<div className="centre">
			<h1 className="title_error">Error 400 - Bad Request</h1>
			<p>Your client has issued a malformed or illegal request.</p>

			<br/>

			<p>Return to the <a href="/" className="link" title="Home">home page</a>?</p>
		</div>
	</>);
}
