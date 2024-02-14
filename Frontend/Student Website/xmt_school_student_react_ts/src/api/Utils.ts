// console.log('result is: ', JSON.stringify(result, null, 4));

export function HandleAPIError(error: Error)
{
	if (error instanceof Error)
	{
		console.log('error message: ', error.message);

		return error.message;
	}
	else
	{
		console.log('unexpected error: ', error);

		return 'An unexpected error occurred';
	}
}

export function InDevMode()
{

	return !process.env.NODE_ENV || process.env.NODE_ENV === 'development'
}

export function GetFormattedDateTime(date: Date)
{
	date = new Date(date);

	const year = date.getFullYear();
	const month = (date.getMonth() + 1).toString().padStart(2, '0');
	const day = date.getDate().toString().padStart(2, '0');
	const hours = date.getHours().toString().padStart(2, '0');
	const minutes = date.getMinutes().toString().padStart(2, '0');

	const formattedDate = `${day}/${month}/${year} ${hours}:${minutes}`;

	return formattedDate;
}
