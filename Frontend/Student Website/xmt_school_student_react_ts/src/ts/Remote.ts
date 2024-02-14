import { getUserByTokenAsync } from '../api/Get';

export default class Database
{
	static getByKeyAsync = async function(Key: string, Params: Array<any>)
	{
		switch(Key)
		{
			case "user":
			{

				return await getUserByTokenAsync(Params[0]);
			}
		}

		throw new Error(`Database.getByKey: ${Key} is an invalid key.`)
	}
}
