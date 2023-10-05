import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

function NotFound() {
    return(
        <Segment placeholder>
            <Header icon>
                <Icon name="search" />
                Not Found
            </Header>
            <Segment.Inline>
                <Button as={Link} to={'/activities'}>
                    Return
                </Button>
            </Segment.Inline>
        </Segment>
    )
}

export default observer(NotFound)