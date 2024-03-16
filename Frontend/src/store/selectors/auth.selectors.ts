import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IAuthState } from "../state/auth.state";

export const getAuthState = createFeatureSelector<IAuthState>('auth');

export const getToken = createSelector(
    getAuthState,
    (state: IAuthState) => state.token
);