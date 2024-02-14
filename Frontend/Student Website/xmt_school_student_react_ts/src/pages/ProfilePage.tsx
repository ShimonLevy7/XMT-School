import Profile from "../components/Profile";
import { CurrentUserData } from "../ts/Users";

export default function ProfilePage()
{

	return (<>
		<Profile currentUser={CurrentUserData}/>
	</>);
}
