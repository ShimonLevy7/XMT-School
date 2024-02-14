import '../styles/About.css';

export default function AboutPage() {

	return (<>
		<header>
			<h1 className='centre'>XMT School - Empowering Futures through Knowledge</h1>
		</header>

		<br />

		<div className='centre' style={{ width: '50vw' }}>
			<section>
				<h2>About XMT</h2>
				<p>XMT stands for Excellence in Mastery and Testing, dedicated to providing a cutting-edge platform for academic growth and assessment. We understand the importance of testing as a vital tool for measuring progress and shaping educational pathways.</p>
			</section>

			<section>
				<h2>Our Values</h2>
				<p>
					<strong>Respecting Every Journey:</strong> At XMT, we honor the uniqueness of each learner's journey. We foster an inclusive environment that respects diverse perspectives, backgrounds, and experiences.
				</p>
				<p>
					<strong>Futuristic Education:</strong> Embracing the future is at the core of XMT's philosophy. We're committed to equipping students with the skills and knowledge needed to thrive in an ever-evolving world.
				</p>
			</section>

			<section>
				<h2>Explore XMT</h2>
				<p>
					<strong>Innovative Testing Solutions:</strong> Discover our state-of-the-art testing platform designed to provide a comprehensive assessment experience. From adaptive testing to insightful analytics, we're here to elevate your learning journey.
				</p>
				<p>
					<strong>Collaborative Learning Community:</strong> Join a community of passionate educators, learners, and professionals who share a common goal—empowering futures through collaborative learning.
				</p>
			</section>

			<footer>
				<p>Join us on the journey of continuous learning and growth at XMT. Your future starts here!</p>
			</footer>
		</div>
	</>);
}
