import React, { useState } from 'react';
import "../custom.css";

export const LanguageDisplay = ({ languageContent, triggerShowLang }) => {

    return (
        <div>
            <div>
                <button className='btn-50 btn-primary4' onClick={triggerShowLang}>Language Display</button>
            </div>
            <div className='box-display box-lang'>
                {/* <textarea className='display' type='text'> */}
                <ul>
                    {/* {console.log("languageContent")}
                    {console.log(languageContent)} */}
                    {(Array.isArray(languageContent)) ? (

                        languageContent.map((item, index) => (
                            <li key={index}>{item}</li>
                        ))

                    ):(
                        ""
                    )}
                </ul>
                {/* </textarea> */}
            </div>

        </div>
    );

}


