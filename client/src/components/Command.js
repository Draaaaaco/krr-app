import React, { useState } from 'react';
import "../custom.css";
import UserManual from './UserManual';
// import LanguageInput from './LanguageInput';



const CommandDiv = ({ currentState, onClick, triggerShowLang }) => {
  //   static displayName = Home.name;
  const [command, setCommand] = useState("");
  const [query, setQuery] = useState("");

  const handleChange = (evt) => {
    setCommand(evt.target.value);
  };
  const handleChangeQuery = (evt) => {
    setCommand(evt.target.value);
  };
  const sendCommand = async () => { await onClick(query);  };
  const sendBuild = async (state) => {
    if (state === "MAIN") { await onClick("new");}
    await onClick(command);
    await onClick("DONE");
    await onClick("build");
    await onClick("continue")
    // await setTimeout(triggerShowLang, 2000)
    // await triggerShowLang();
  };
  const sendAbort = async (state) => {
        setCommand("");

    if (state === "CREATE") { await onClick("ABORT"); }
    await onClick("new");
  }
  const abortQuery = () =>{
    setQuery("");
  }

  return (
    <div>
      {(currentState === "CREATE") || (currentState === "MAIN") ? (
        <div>
          <div >
            <textarea className='command-input' type='text' onChange={handleChange} value={command} />
            
            {/* <LanguageInput /> */}
          </div>
          <div>
            <button className='btn-alert btn-50' onClick={() => sendAbort(currentState)}>Abort</button>
            {/* <button className='btn-primary btn-33' onClick={sendCommand}>Send</button> */}
            <button className='btn-finish btn-50' onClick={() => sendBuild(currentState)}>Build</button>
          </div>
          <div className='command-input'>
              <UserManual currentState={currentState} />
            </div>
        </div>
      ) : (currentState === "QUERY") ? (
        <div>
          <div>
            <textarea className='command-input' type='text' onChange={handleChangeQuery} value={query}/>
          </div>
          <div>
            <button className='btn-alert btn-50' onClick={abortQuery}>Abort</button>
            <button className='btn-primary btn-50' onClick={sendCommand}>Send Query</button>
          </div>
          <div className='command-input'>
              <UserManual currentState={currentState} />
            </div>
        </div>
      ) : (<div></div>)}



    </div>
  );
}
export default CommandDiv
