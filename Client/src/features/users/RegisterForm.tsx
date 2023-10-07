import { observer } from "mobx-react-lite"
import { ErrorMessage, Form, Formik } from 'formik'
import MyTextInput from "../../app/common/form/MyTextInput"
import { Button, Header, Label } from "semantic-ui-react"
import { useStore } from "../../app/stores/store"
import * as Yup from 'yup'
import ValidationError from "../errors/ValidationError"

export default observer(function RegisterForm() {
    class InitialValues {
        displayName: string = "";
        username: string = "";
        email: string = '';
        password: string = '';
        error: string[] = []
    }

    const validationSchema = Yup.object({
        displayName: Yup.string().required(),
        username: Yup.string().required(),
        email: Yup.string().required(),
        password: Yup.string().required(),
    })

    const {userStore} = useStore();

    return (
        <Formik
            initialValues={new InitialValues()}
            onSubmit={(values, {setErrors}) => userStore.register(values).catch(error => setErrors({error}))}
            validationSchema={validationSchema}
        >
            {(formikProps) => (
                <Form 
                    className="ui form error" 
                    onSubmit={formikProps.handleSubmit}
                    autoComplete="off"
                >
                    <Header as="h2" content="Activities User Registration" color="teal" textAlign="center" />
                    <MyTextInput placeholder="Display Name" name="displayName" />
                    <MyTextInput placeholder="Username" name="username" />
                    <MyTextInput placeholder="Email" name="email" />
                    <MyTextInput placeholder="Password" name="password" />
                    <ErrorMessage 
                        name="error"
                        render={() => <ValidationError errors={formikProps.errors.error as string[]}/>}
                    />
                    <Button 
                        disabled={!formikProps.isValid || !formikProps.dirty || formikProps.isSubmitting}
                        loading={formikProps.isSubmitting} 
                        positive 
                        content="Register" 
                        type="submit" 
                        fluid 
                    />
                </Form>
            )}
        </Formik>
    )
})