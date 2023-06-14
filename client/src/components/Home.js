import React, { useState, useEffect } from 'react';
import { StateDiv } from "./StateDiv";
import { CommandDiv } from './Command';
import { LanguageDisplay } from "./LanguageDisplay";
import { ModelDisplay } from './ModelDisplay';

export const Home = () => {
  // static displayName = Home.name;
  // constructor(props) {
  //   super(props);
  //   this.state = { currentState: "", };
  // } 
  const [currentState, setCurrentState] = useState("");
  const [responeContent, setResponeContent] = useState("");
  const [language, setLanguage] = useState([]);
  const [ifSent, setIfSent] = useState(0);
  const fetchState = async () => {
    // Send a GET requesthttp://localhost:5271/knowledge/state
    await fetch('http://localhost:5271/knowledge/state', {
      method: 'GET',
    })
      .then(response => {
        // Update state with the received data
        return response.json();
      })
      .then(data => {
        setCurrentState(data.state);
        console.log(currentState);
      })
      .catch(error => {
        console.error(error);
      });
  };

  const sendCommand = async (command) => {
    // console.log(command);
    await fetch('http://localhost:5271/knowledge/command', {
      method: 'POST',
      // mode: 'no-cors',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        // 'Access-Control-Allow-Origin': '*'
      },
      body: JSON.stringify({ text: command })
    }).then(response => {
      // Update state with the received data
      return response.json();
    }).then(data => {
      setResponeContent(data.text);

      if (data.status === "INFO_LANG") {
        setLanguage(data.text)
      }
      console.log(data.status);

      console.log(data.text);
    }).catch(error => {
      console.error(error);
    });
    await fetchState();
    // console.log(currentState);
    setIfSent((ifSent + 1) % 100);
  };

  useEffect(() => { fetchState() }, [ifSent]);
  // useEffect(() => { fetchState() })
  const languageLabel = async () => {
    if (currentState === "MAIN") {
      await sendCommand("continue");
    } else if (currentState === "CREATE") {
      await fetchState();
    } else {
      await sendCommand("DONE");
      await sendCommand("continue");

    }
  };

  const queryLabel = async () => {
    if (currentState === "MAIN") {
      await sendCommand("query");
    } else if (currentState === "CREATE") {
      await sendCommand("DONE\nbuild\nquery");
    } else {
      await fetchState();
    }
  };
  const showLang = async () => {
    await fetchState();
    console.log(currentState);
    if (currentState === "MAIN") {
      await sendCommand("show lang");
      // setLanguage(responeContent);
      // console.log(responeContent);
    }else if (currentState ==="CREATE") {
      // await sendCommand("DONE");
      // await sendCommand("show lang");
      // await sendCommand("continue");
    }
  };
  return (
    <div>
      <StateDiv currentState={currentState} refresh={fetchState} onClick={sendCommand} />
      <div className="main-panel">
        <div>
          <div>
            <button className='btn-primary btn-50' onClick={languageLabel}>Language </button>
            <button className='btn-primary2 btn-50' onClick={queryLabel}>Query </button>
          </div>
          <CommandDiv currentState={currentState} onClick={sendCommand} triggerShowLang={showLang} />
        </div>
        <div>
          <LanguageDisplay languageContent={language} triggerShowLang={showLang} />
        </div>
        <div>
          <ModelDisplay />
        </div>
      </div>
    </div>
  );
}
