import React, { useState } from 'react';
import "../custom.css";



export const CommandDiv = ({ currentState, onClick, triggerShowLang }) => {
  //   static displayName = Home.name;
  const [command, setCommand] = useState("");

  const handleChange = (evt) => {
    setCommand(evt.target.value);
  };

  const sendCommand = async () => { onClick(command); };
  const sendBuild = async () => {
    await onClick(command);
    await onClick("DONE");
    await onClick("build");
    await setTimeout(triggerShowLang, 2000)

    // await triggerShowLang();
  };
  const sendAbort = async () => {
    await onClick("ABORT");
    await onClick("new");
  }

  return (
    <div>
      {(currentState === "CREATE") || (currentState === "MAIN") ? (
        <div>
          <div>
            <textarea className='command-input' type='text' onChange={handleChange} />
          </div>
          <div>
            <button className='btn-finish btn-33' onClick={sendAbort}>Abort</button>

            <button className='btn-primary btn-33' onClick={sendCommand}>Send</button>
            <button className='btn-finish btn-33' onClick={sendBuild}>Build</button>
          </div>
        </div>
      ) : (currentState === "QUERY") ? (
        <div>
          <div>
            <textarea className='command-input' type='text' onChange={handleChange} />
          </div>
          <div>
            <button className='btn-primary btn-50' onClick={sendCommand}>Send</button>
            <button className='btn-finish btn-50' onClick={sendBuild}>Build</button>
          </div>
          {/* <button className='btn-finish' onClick={sendBuild}>Done</button> */}
        </div>
      ) : (<div></div>)}



    </div>
  );
}
