import { IAuthState, initialAuthState } from './../state/auth.state';
import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './../actions/auth.actions';

export const authReducer = createReducer(
    initialAuthState,
    on(AuthActions.setToken, (state, { token }) => ({ ...state, token })),
    on(AuthActions.clearToken, state => ({ ...state, token: null })),
);
