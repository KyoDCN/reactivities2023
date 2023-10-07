import { observer } from "mobx-react-lite";
import { Message } from "semantic-ui-react";

interface Props {
    errors: string[] | undefined;
}

export default observer(function ValidationError({errors}: Props) {
    return (
        <Message error>
            {errors && (
                <Message.List>
                    {errors.map((err: string, i) => (
                        <Message.Item key={i}>{err}</Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>        
    )
});