// import { useCallback } from 'react';
import { Handle, Position } from 'reactflow';
import '../custom.css';

const handleStyles = {
    right: { left: 95 },
    left: { left: 55},
    top: { top: 20 },
    bottom: { top: 40 }
};

export const  MultiHandleNode = ({ data }) => {

    return (
        <div className='multiHandle-node'>
            <Handle type="target" position={Position.Top} id='top-T'style={handleStyles.right}/>
            <Handle type="source" position={Position.Top} id='top-S'style={handleStyles.left}/>
            <Handle type="target" position={Position.Left} id='left-T' style={handleStyles.top}/>
            <Handle type="source" position={Position.Left} id='left-S' style={handleStyles.bottom}/>
            <div >
                {data.label}
            </div>
            <Handle type="target" position={Position.Right} id='right-T'style={handleStyles.bottom}/>
            <Handle type="source" position={Position.Right} id='right-S'style={handleStyles.top}/>
            <Handle type="target" position={Position.Bottom} id="bottom-T" style={handleStyles.left}/>
            <Handle type="source" position={Position.Bottom} id="bottom-S" style={handleStyles.right} />
        </ div>
    );
}
export default MultiHandleNode;