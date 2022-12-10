import { createFeatureSelector, createSelector } from '@ngrx/store';

import * as auth from './reducers/auth.reducers';

export const reducers = {
  auth: auth.authReducer
};

export const selectAuthState = createFeatureSelector<auth.State>('auth');

export const selectStateToken = createSelector(
  selectAuthState,
  state => state.user?.accessToken
);

export const selectStateAuthStatus = createSelector(
  selectAuthState,
  (state:auth.State) => state.isAuthenticated
);

export const selectStateUser = createSelector(
  selectAuthState,
  state => state.user
);

export const selectUserId = createSelector(
  selectAuthState,
  state => state.user?.id
);

export const selectUserRole = createSelector(
  selectAuthState,
  state => state.user?.role
);

export const selectStateErrorMessage = createSelector(
  selectAuthState,
  state => state.errorMessage
);

