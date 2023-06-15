import React, { useMemo } from 'react';
// import SelfConnecting from './SelfConnecting';
import ReactFlow, { MiniMap, Controls } from "reactflow"
import MultiHandleNode from "./MultiHandleNode.js"
import Edge from './Edge.js';

// import { ConnectionData, NodeData } from "flowchart-react/dist/schema";
import 'reactflow/dist/style.css';

export const nodeTypes = { textUpdater: MultiHandleNode };
export const edgeTypes = { myEdge: Edge };

export const colorRainbow = [
    '#FF8E00', '#187498', '#FF1700', '#3F52E3', '#000000',
]
const Legend = ({actions}) => {
    return (
        <div className="legend">
            
            
                {(Array.isArray(actions)) ? (
                    actions.map((value, index) => (
                        
                            <p style={{color:colorRainbow[index]}}>{value+" -----"}</p>
                        
                    ))
                ) : ('')}
            
        </div>
    );
};

function ModelFlow({ nodesInfo, edgesInfo, connections }) {
    // const edgeTypes = useMemo(() => ({ special: SelfConnecting }), []);
    // const initialEdges = [{
    //     id: 'e1-2', source: '2', target: '2', label: 'edge label 1', sourceHandle: 'top-S', targetHandle: 'left-T', type: 'myEdge'
    // }, {
    //     id: 'e2-1', source: '1', target: '1', label: 'edge label 2', sourceHandle: 'top-S', targetHandle: 'left-T', type: 'myEdge', style: { stroke: 'red', strokeWidth: 3 }, markerEnd: { type: "arrow" }
    // }];
    const fNodeTypes = useMemo(() => (nodeTypes), []);
    const fEdgeTypes = useMemo(() => (edgeTypes), []);

    const findHandle = (source, target) => {
        var sourceC = (source % 2);
        var targetC = (target % 2);
        var sourceR = Math.floor(source / 2);
        var targetR = Math.floor(target / 2);
        if (sourceC !== targetC) {
            return { source: (sourceC === 0) ? 'right-S' : 'left-S', target: (targetC === 0) ? 'right-T' : 'left-T' };
        } else if (sourceR === targetR) {
            return { source: "top-S", target: 'left-T' };
        } else if ((sourceR > targetR) && (sourceR === targetR + 1)) {
            return { source: 'top-S', target: 'bottom-T' };
        } else if ((sourceR > targetR) && (sourceR > targetR + 1)) {
            return { source: (sourceC === 0) ? 'left-S' : 'right-S', target: (targetC === 0) ? 'left-T' : 'right-T' };
        } else if ((sourceR < targetR) && (sourceR + 1 === targetR)) {
            return { source: 'bottom-S', target: 'top-T' };
        } else if ((sourceR < targetR) && (sourceR + 1 < targetR)) {
            return { source: (sourceC === 0) ? 'left-S' : 'right-S', target: (targetC === 0) ? 'left-T' : 'right-T' };
        }
    }

    return (
        <div className='box-display box-model'>
            {/* // <Flowchart /> */}
            {/* {console.log(nodesInfo)} */}
            <ReactFlow
                readOnly={true}
                nodeTypes={fNodeTypes}
                edgeTypes={fEdgeTypes}
                // nodes = {initialNodes}
                nodes={nodesInfo.map((value, index) => ({
                    id: index.toString(),
                    position: { x: 80 + (index % 2) * 300, y: 80 + Math.floor(index / 2) * 150 },
                    data: { label: value },
                    type: 'textUpdater',
                    // type: 'bidirectional',
                    // sourcePosition: Position.Right,
                    // targetPosition: Position.Left,
                }))}
                edges={connections.flatMap((row, rowIndex) =>
                    row.map((value, columnIndex) => ({
                        id: rowIndex + '-' + columnIndex,
                        source: rowIndex.toString(),
                        target: value.toString(),
                        sourceHandle: findHandle(rowIndex, value).source,
                        targetHandle: findHandle(rowIndex, value).target,
                        type: 'myEdge',
                        style: { stroke: colorRainbow[columnIndex], strokeWidth: 3 },
                        markerEnd: { type: "arrow", color: colorRainbow[columnIndex] },
                        data: { label: edgesInfo[columnIndex] },
                    }
                    )))
                }
            >
                <Controls />
                <MiniMap className='minimap' nodeStrokeWidth={1} zoomable pannable />
                <Legend actions={edgesInfo}/>

            </ ReactFlow>
        </div>
    );
}
export default ModelFlow;