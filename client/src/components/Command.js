import React, { useState } from 'react';
import "../custom.css";
import UserManual from './UserManual';
// import LanguageInput from './LanguageInput';



const CommandDiv = ({ currentState, onClick, triggerShowLang }) => {
  //   static displayName = Home.name;
  const [command, setCommand] = useState("");

  const handleChange = (evt) => {
    setCommand(evt.target.value);
  };

  const sendCommand = async () => { onClick(command); };
  const sendBuild = async (state) => {
    if (state === "MAIN") { return; }
    await onClick(command);
    await onClick("DONE");
    await onClick("build");
    // await setTimeout(triggerShowLang, 2000)

    // await triggerShowLang();
  };
  const sendAbort = async (state) => {
    if (state === "CREATE") { await onClick("ABORT"); }
    await onClick("new");
  }

  return (
    <div>
      {(currentState === "CREATE") || (currentState === "MAIN") ? (
        <div>
          <div >
            <textarea className='command-input' type='text' onChange={handleChange} />
            <div className='command-input'>
              <UserManual currentState={currentState} />
            </div>
            {/* <LanguageInput /> */}
          </div>
          <div>
            <button className='btn-alert btn-33' onClick={() => sendAbort(currentState)}>Abort</button>
            <button className='btn-primary btn-33' onClick={sendCommand}>Send</button>
            <button className='btn-finish btn-33' onClick={() => sendBuild(currentState)}>Build</button>
          </div>
        </div>
      ) : (currentState === "QUERY") ? (
        <div>
          <div>
            <textarea className='command-input' type='text' onChange={handleChange} />
            <div className='command-input'>

              <UserManual currentState={currentState} />
            </div>
          </div>
          <div>
            <button className='btn-primary btn-100' onClick={sendCommand}>Send</button>
          </div>
        </div>
      ) : (<div></div>)}



    </div>
  );
}
export default CommandDiv
