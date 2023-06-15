import React from 'react';
import { BaseEdge, BezierEdge, EdgeLabelRenderer, getBezierPath } from 'reactflow';

export default function Edge(props) {
    // we are using the default bezier edge when source and target ids are different



    const { sourceX, sourceY, targetX, targetY, id, markerEnd, style, sourcePosition, targetPosition, data } = props;
    // const prefix = Math.random() * 20;
    const radiusX = (sourceX - targetX) * 0.6 ;
    const radiusY = (sourceY - targetY) * 1.4 ;
    const edgePath_self = `M ${sourceX - 5} ${sourceY} A ${radiusX} ${radiusY} 0 1 0 ${targetX + 2
        } ${targetY}`;

    const [edgePath_default, labelX, labelY] = getBezierPath({
        sourceX,
        sourceY,
        sourcePosition,
        targetX,
        targetY,
        targetPosition,
    });
    return (<>
        <BaseEdge path={(props.source !== props.target) ? edgePath_default : edgePath_self} markerEnd={markerEnd} style={style} />
        <EdgeLabelRenderer>
            <div
                style={{
                    position: 'absolute',
                    transform: `translate(-50%, -70%) translate(${labelX}px,${labelY}px)`,
                    // background: '#ffcc00',
                    padding: 10,
                    borderRadius: 5,
                    fontSize: 10,
                    fontWeight: 500,
                }}
                className="nodrag nopan"
            >
                {data.label}
            </div>
        </EdgeLabelRenderer>
    </>);
}
