import React from 'react';
import { TiRefresh } from 'react-icons/ti';

export const StateDiv = ({ currentState, refresh }) => {
    // const [currentState, setCurrentState] = useState("Ma");


    // useEffect(fetchState);
    // const sendNew = () => { onClick('new') };
    // const sendShow = () => { onClick('show') };
    // const sendQuery = () => { onClick('query') };
    // const sendBuild = () => { onClick('build') };
    // const sendContinue = () => { onClick('continue') };

    return (
        <div>
            <div>
                <h1>Current State: <font color='#185864'>{currentState}</font> <button className='btn-transparent' onClick={refresh}><TiRefresh /></button></h1>
                {/* Button to trigger the GET request */}
                {/* <button onClick={fetchData}>Update Status</button> */}
            </div>
            {/* <div>
                {(currentState === "CREATE")  ? (
                    <div>
                        <button>Button 1</button>
                        <button>Button 2</button>
                    </div>
                ) : (
                    <div>
                        <button>Button 3</button>
                        <button>Button 4</button>
                    </div>
                )}
                <button onClick={sendNew}>New Language</button>
                <button onClick={sendShow}>Show</button>
                <button onClick={sendQuery}>Query</button>
                <button onClick={sendBuild}>Build</button>
                <button onClick={sendContinue}>continue</button>
            </div> */}
        </div>
    );
};

