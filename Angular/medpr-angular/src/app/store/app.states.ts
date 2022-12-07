import { createFeatureSelector, createSelector } from '@ngrx/store';

import * as auth from './reducers/auth.reducers';

export interface AppState {
  authState: auth.State;
}

export const reducers = {
  auth: auth.authReducer
};

export const selectAuthState = createFeatureSelector<AppState>('auth');

export const selectStateToken = createSelector(
  selectAuthState,
  state => state.authState.user?.accessToken
);

export const selectStateAuthStatus = createSelector(
  selectAuthState,
  state => state.authState.isAuthenticated
);

export const selectStateUser = createSelector(
  selectAuthState,
  state => state.authState.user
);

export const selectStateErrorMessage = createSelector(
  selectAuthState,
  state => state.authState.errorMessage
);

