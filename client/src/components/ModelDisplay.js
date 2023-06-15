import React, { useState } from 'react';
import Select from 'react-select';
import "../custom.css";
import ModelFlow from './ModelFlow';

 const ModelDisplay = ({ modelContent, triggerShowModel }) => {
    const customStyles = {
        control: base => ({
            ...base,
            height: '50px',
            minHeight: '50px',
            // background: '#',
            borderWidth: '3px',
            borderColor: '#354750'
        })
    };
    const [modelId, setModelId] = useState(0);
    const selectModel = (selectedOption) => (setModelId(selectedOption.value));

    return (
        <div>
            <div>
                <button className='btn-50 btn-primary3' onClick={triggerShowModel}>Model Display</button>
                {(Array.isArray(modelContent)) ? (
                    <Select

                        className="basic-single"
                        classNamePrefix="select"
                        options={modelContent.map((item, index) => (
                            { label: "Model " + index, value: index }
                            // <li key={index}>{JSON.parse(item).Actions[0]}</li>
                        ))}
                        onChange={selectModel}
                        styles={customStyles}
                    />
                ) : <div />}
            </div>
            <div >
                {/* <textarea readOnly className='display-model' type='text'>
                    {modelContent}
                </textarea> */}
                { }
                {(Array.isArray(modelContent)) && (modelId < modelContent.length ) ? (
                    console.log(JSON.parse(modelContent[modelId]).States)):("")}
                {(Array.isArray(modelContent)) && (modelId < modelContent.length) ? (
                    
                    <ModelFlow
                        nodesInfo={JSON.parse(modelContent[modelId]).States}
                        edgesInfo={JSON.parse(modelContent[modelId]).Actions}
                        connections={JSON.parse(modelContent[modelId]).TransitionMat}
                    />
                ) : (<div className='box-display box-model' />)
                }
            </div>

        </div>
    );
}


export default ModelDisplay;