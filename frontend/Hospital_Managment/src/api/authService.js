import axiosInstance from "./axiosInstance";

export const authService = {
    //Post /api/logjn  //Arvind Pandey...
    login : (Credentials)=> axiosInstance.post('/Authentication',Credentials), 
};
 
 