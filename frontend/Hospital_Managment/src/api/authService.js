import axiosInstance from "./axiosInstance";

export const authService = {
    //Post /api/logjn  //Arvind.
    login : (Credentials)=> axiosInstance.post('/Authentication',Credentials), 
};
 
 