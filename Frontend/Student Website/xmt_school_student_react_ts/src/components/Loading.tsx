import '../styles/Loading.css';

export default function Loading()
{

	return (<>
		<div className="loading_centre">
			<div className="spinner-border" role="status">
				<span className="sr-only">Loading...</span>
			</div>
		</div>
	</>);
}
