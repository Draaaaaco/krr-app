import React from 'react';
import ReactMarkdown from 'react-markdown'
import "../custom.css";

const statementManual = [
    "#### Statement Manual",
    "----------",
    "1. Initial Value Statement\n\n",
    "\t**initially** {FLUENT} \n\n",
    "2. Effect Statement\n\n",
    "\t{ACTION} **causes** {FLUENT1 ...} [**if** {CONTDITIO1 ...}]\n\n",
    "3. Value Statement\n\n",
    "\t{FLUENT} **after** {ACTION1,...}",
    "4. Action Time Statement\n\n",
    "\t{FLUENT} **lasts** {TIME(numeric)}",
];

const queryManual = [
    "#### Query Manual",
    "----------",
    "1. Condition Query Statement\n\n",
    "\t{FLUENT1 ...} **holds after** {ACTION1,...} \n\n",
    "2. Time Realization Query Statement\n\n",
    "\t{TIME(numeric)} **suffices** {ACTION1,...}\n\n",
    
];


const UserManual = ({ currentState }) => {
    return (

        <div >
            
            {(currentState === "CREATE") || (currentState === "MAIN") ? (
                <ReactMarkdown >
                    {statementManual.join('\n')}
                </ReactMarkdown>
            ) : (
                <ReactMarkdown >
                    {queryManual.join('\n')}
                </ReactMarkdown>
            )}
            
        </div>
    );

}

export default UserManual;