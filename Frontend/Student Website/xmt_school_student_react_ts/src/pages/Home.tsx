import '../styles/Home.css';

export default function HomePage() {

	return (<>
		<header>
			<h1>XMT School - Recent News</h1>
		</header>

		<section className="news-container">
			<div className="news-item">
				<h2>Exciting Developments in XMT's Robotics Program</h2>
				<p>Students at XMT are gearing up for an immersive experience in robotics! Our state-of-the-art robotics program is set to launch next month, providing hands-on learning and fostering innovation.</p>
				<p><em>Published on: February 2, 2024</em></p>
			</div>

			<div className="news-item">
				<h2>Mock Trial Success: XMT Debaters Shine in Regional Competition</h2>
				<p>XMT's debate team triumphed at the regional mock trial competition, showcasing their exceptional skills in argumentation and critical thinking. Congratulations to the team on this remarkable achievement!</p>
				<p><em>Published on: January 28, 2024</em></p>
			</div>

			<div className="news-item">
				<h2>Breaking Ground: XMT's New STEM Building</h2>
				<p>XMT is proud to announce the groundbreaking ceremony for our new STEM building. This cutting-edge facility will provide students with advanced resources for science, technology, engineering, and mathematics education.</p>
				<p><em>Published on: January 20, 2024</em></p>
			</div>
		</section>

		<br/>

		<footer className='centre'>
			<p>Stay tuned for more exciting updates and happenings at XMT School!</p>
		</footer>
	</>);
}
