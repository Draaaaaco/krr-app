
import UserManual from "./UserManual";

const Manual = () => {
    return (

        <div >
            
            <UserManual currentState={"MAIN"}/>
            <UserManual currentState={"QUERY"}/>

            
        </div>
    );

}

export default Manual;