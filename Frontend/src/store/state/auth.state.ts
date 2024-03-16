import * as fromRoot from './app.state';

export interface IAuthState extends fromRoot.State {
    token: string | null;
}

// auth.state.ts
export const initialAuthState: IAuthState = {
    token: null,
};


