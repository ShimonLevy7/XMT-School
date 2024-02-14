import "datatables.net-dt/css/jquery.dataTables.css";
import '../styles/ListTests.css';

import { useEffect, useLayoutEffect, useRef } from "react";

import { FaBan, FaEye, FaPencilAlt, FaQuestion } from "react-icons/fa";

import DataTable, { Config, ConfigColumnDefs } from 'datatables.net';
import 'datatables.net-responsive';
import 'datatables.net-dt/css/jquery.dataTables.css';

import { ITestWithStudentInfo } from "../api/Types";
import { GetFormattedDateTime } from "../api/Utils";
import { goToTestUrl } from "../utils/TestsUtils";

export default function ListTests({ testsWithStudentInfo }: { testsWithStudentInfo: ITestWithStudentInfo[] }) {
	// Init Database.net table.
	const tableRef = useRef<HTMLTableElement | null>(null); // Reference to the table.

	useLayoutEffect(() => {
		// Initialize DataTable only if the table hasn't been initialized
		if (tableRef.current && !tableRef.current.classList.contains('dataTable')) {
			const dataTable = new DataTable(tableRef.current, {
				responsive: true,
				lengthChange: true,
				autoWidth: true,
				columnDefs: [
					{ responsivePriority: 2, targets: 0 } as ConfigColumnDefs,
					{ responsivePriority: 1, targets: 1 } as ConfigColumnDefs,
					{ responsivePriority: 1, targets: 2, searchable: false } as ConfigColumnDefs,
					{ responsivePriority: 0, targets: 3 } as ConfigColumnDefs,
					{ responsivePriority: 1, targets: 4 } as ConfigColumnDefs,
					{ responsivePriority: 2, targets: 5, searchable: false } as ConfigColumnDefs,
					{ responsivePriority: 3, targets: 6, searchable: false } as ConfigColumnDefs,
					{ responsivePriority: 0, targets: 7, searchable: false } as ConfigColumnDefs,
					{ responsivePriority: 0, targets: 8, searchable: false } as ConfigColumnDefs
				] as ConfigColumnDefs[]
			} as Config);

			return () => {
				// Destroy the DataTable instance when the component unmounts
				dataTable.destroy();
			};
		}
	}, [testsWithStudentInfo]); // Empty dependency array ensures that this effect runs only once

	// Used to allow pressing buttons without the onClick attribute.
	useEffect(() => {
		const handleClick = (event: MouseEvent) => {
			// Check if the clicked element is a button
			if ((event.target as HTMLElement).tagName !== 'BUTTON')
				return;

			const button: HTMLButtonElement = event.target as HTMLButtonElement;

			// Find the class that matches the pattern "testButton-<ID>"
			const regex = /^testButton-(\d+)$/;

			for (const className of Array.from(button.classList)) {
				const match = className.match(regex);

				if (match) {
					const id: number = Number.parseInt(match[1]);

					goToTestUrl(testsWithStudentInfo[id]);

					break; // Exit the loop after the first match
				}
			}
		};

		// Add click event listener to the document
		document.addEventListener('click', handleClick);

		// Cleanup the event listener on component unmount
		return () => {
			document.removeEventListener('click', handleClick);
		};
	}, [testsWithStudentInfo]); // Empty dependency array ensures the effect runs once on mount

	return (
		<>
			<div className="w-75 centre">
				<h3 className='text-center m-4'>Take a Test</h3>

				<table ref={tableRef} className="display responsive" style={{ width: "100%" }}>
					<thead>
						<tr>
							<th>Test #</th>
							<th>Status</th>
							<th>Mark</th>
							<th>Subject</th>
							<th>Teacher</th>
							<th>Questions</th>
							<th>Created On</th>
							<th>Starts On</th>
							<th>Ends On</th>
							<th></th>
						</tr>
					</thead>
					<tbody>
						{testsWithStudentInfo.map((test, i) => {
							const now = Date.now()

							const status = test.wasTaken // Was taken?
								? 'Taken' // Yes, taken.
								: test.canTake // No, can take?
									? 'Ongoing' // Yes, ongoing.
									: now > new Date(test.test.endDate).getTime() // Can't take, wasn't taken.
										? 'Expired' // Now > end date? Means test expired.
										: 'Upcoming'; // Only remaining option is upcoming.

							const icon = test.canTake
								? FaPencilAlt({ size: 16 })
								: test.wasTaken
									? FaEye({ size: 16 })
									: FaBan({ size: 16 });

							const buttonText = test.canTake
								? ' Take the Test'
								: test.wasTaken
									? ' View Results'
									: '';

							return (<tr key={`test-${i}`}>
								<td>{test.test.id}</td>
								<td>{status}</td>
								<td>
									{test.wasTaken
										? `${test.mark.points}/100`
										: ''}
								</td>
								<td>{test.test.name}</td>
								<td>{test.authorName}</td>
								<td>{test.test.questions.length > 0 ? test.test.questions.length : FaQuestion({ size: 16 })}</td>
								<td>{GetFormattedDateTime(test.test.creationDate)}</td>
								<td>{GetFormattedDateTime(test.test.startDate)}</td>
								<td>{GetFormattedDateTime(test.test.endDate)}</td>
								<td>
									<button disabled={!test.canTake && !test.wasTaken} className={`testButton-${i} btn btn-block ${test.canTake ? 'btn-primary' : test.wasTaken ? 'btn-info' : 'btn-secondary'}`}>
										{icon} {buttonText}
									</button>
								</td>
							</tr>);
						})}
					</tbody>
				</table>
			</div>
		</>
	);
}
