import React, { useState } from 'react';
import "../custom.css";

export const ModelDisplay = ({ modelContent }) => {
    return (
        <div>
            <div>
                <button className='btn-50 btn-primary3'>Model Display</button>
            </div>
            <div className='box-display box-model'>
                {/* <textarea readOnly className='display-model' type='text'>
                    {modelContent}
                </textarea> */}
            </div>

        </div>
    );

}


